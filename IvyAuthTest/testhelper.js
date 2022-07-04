const bindir=process.env.IVYAUTH_BINDIR;
const fs = require('fs')
const axios = require('axios');
const {spawn} = require('child_process');
const assert = require('assert')

var serverProcess = null

function startServerWithSettings(settings) {
    describe('StartServer', function(){
        it ('Copy test settings', async function () {
          assert.ok(await copySettings('testsettings.json'));
        });
        it ('Start server', async function () {
          assert.ok(await launchServer());
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

async function makeRequest(requestBody) {
    return await axios.post('http://localhost:5000/api/GenerateToken', requestBody).catch(function(error) {
        if (error.response) {
            return {
                status: error.response.status,
                data: error.response.data,
            };
        }
        throw error;
    });
}

function copySettings(settings) {
    return new Promise(resolve => {
        if (bindir == undefined || bindir.length ==0) {
            console.log ('CopySettings Error: IVYAUTH_BINDIR not set.');
            resolve(false);
        }
        else {
            fs.copyFile(settings, bindir + '/appsettings.json', (err) => {
                if(err)
                {
                    console.log('CopySettings Error: ', err);
                    resolve(false)
                }
                else
                {
                    resolve(true);
                }
            });
        }
    });
}

function launchServer(settings) {
    return new Promise(resolve => {
        serverProcess = spawn(bindir + '/IvyAuth', [], {'cwd': bindir})

        setTimeout(() => resolve(true), 1500);
    });
}

module.exports = {startServerWithSettings, ping, makeRequest, stopServer}
