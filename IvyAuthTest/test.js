var assert = require('assert');
var testHelper = require('./testhelper');

testHelper.startServerWithSettings('testtesttings.json');

const requestBodyValidCreds = {
  userName: 'testuser1',
  passWord: 'appleOrange34'
};

const requestBodyInvlaidCreds = {
  userName: 'badusername',
  passWord: 'badpassword'
}

describe('Ping', function () {
  it('Ping succeeds', async function() {
    const res = await testHelper.ping();
    assert.equal(res.status, 200, 'Request has 200 status');
    console.log(res.data);
    // todo verify build numbers
  });
});

describe('GenerateToken', function () {
  describe('With valid credentials', function () {
    it('should return valid token', async function () {
      const res = await testHelper.makeRequest(requestBodyValidCreds);
      assert.equal(res.status, 200, 'Request has 200 status');
      // todo verify token
    });
  });
  describe('With invalid credentials', function () {
    it('should return 401', async function () {
      const res = await testHelper.makeRequest(requestBodyInvlaidCreds);
      console.log('teststatus', res.status);
      assert.equal(res.status, 401, 'Request has 4011 status');
    });
  });
});
describe('Stop server', function() {
  it('Server stopped', function () {
    assert.ok(testHelper.stopServer());
  });
});