const bindir=process.env.IVYAUTH_BINDIR;
const fs = require('fs-extra')
const axios = require('axios');
const {spawn} = require('child_process');
const assert = require('assert');
const { VerifierAsync } = require('fast-jwt');

var serverProcess = null

function startServerWithSettings(settings) {
    const targetDir = '/tmp/ivyAuthTestRun';

    describe('StartServer', function(){
        it ('Setup test run dir', async function() {
            assert.ok(await setupTestRunDir(targetDir));
        });
        it ('Copy binaries', async function () {
            assert.ok(await copyBins(targetDir));
        });
        it ('Copy test settings', async function () {
          assert.ok(await copySettings('testsettings.json', targetDir));
        });
        it ('Start server', async function () {
          assert.ok(await launchServer(targetDir));
        });
      });
}

function stopServer() {
    serverProcess.kill();
    serverProcess = null;
    return true;
}

async function ping() {
    return await axios.get('http://localhost:5000/api/ping');
}

async function makeGetRequest(api, requestBody) {
    return await axios.get('http://localhost:5000/api/' + api, requestBody).catch(function(error) {
        if (error.response) {
            return {
                status: error.response.status,
                data: error.response.data,
            };
        }
        throw error;
    });
}
async function makePostRequest(api, requestBody) {
    return await axios.post('http://localhost:5000/api/' + api, requestBody).catch(function(error) {
        if (error.response) {
            return {
                status: error.response.status,
                data: error.response.data,
            };
        }
        throw error;
    });
}

function setupTestRunDir(targetDir)
{
    return new Promise(resolve => {
        fs.emptyDir(targetDir)
        .then(() => {
            resolve(true)
        })
        .catch( err => {
            console.log('EmptyDir Error:', err);
            resolve(false);
        });
    });
}

function copyBins(targetDir) {
    return new Promise(resolve => {
        if (bindir == undefined || bindir.length ==0) {
            console.log ('CopySettings Error: IVYAUTH_BINDIR not set.');
            resolve(false);
        }
        else {
            fs.copy(bindir, targetDir)
            .then(() => {
                resolve(true);
            })
            .catch( err => {
                console.log('CopyBins Error:', err);
                resolve(false);
            });
        }
    });
}

function copySettings(settingsFile, targerDir) {
    return new Promise(resolve => {
        if (bindir == undefined || bindir.length ==0) {
            console.log ('CopySettings Error: IVYAUTH_BINDIR not set.');
            resolve(false);
        }
        else {
            fs.copyFile(settingsFile, targerDir + '/appsettings.json')
            .then(() => {
                resolve(true);
            })
            .catch(err => {
                console.log("Copy Settings Error: ", err);
                resolve(false);
            });
        }
    });
}

function launchServer(targetDir) {
    return new Promise(resolve => {
        serverProcess = spawn(targetDir + '/IvyAuth', [], {'cwd': targetDir})

        serverProcess.stdout.on('data', (data) => {
            console.log(`server stdout: [[${data}]]`);
          });
        serverProcess.stderr.on('data', (data) => {
            console.log(`server stderr: [[${data}]]`);
          });

        setTimeout(() => resolve(true), 10000);
    });
}

module.exports = {startServerWithSettings, ping, makeGetRequest, makePostRequest, stopServer}
