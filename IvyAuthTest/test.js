var assert = require('assert');
var { createDecoder } = require('fast-jwt');
var testHelper = require('./testhelper');

testHelper.startServerWithSettings('testtesttings.json');

const requestBodyValidCreds = {
  userId: 'testuser1@test.com',
  passWord: 'appleOrange34'
};

const requestBodyInvlaidCreds = {
  userId: 'badusername',
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
      const res = await testHelper.makePostRequest('GenerateToken', requestBodyValidCreds);
      assert.equal(res.status, 200, 'Request has 200 status');
      
      const decoder = createDecoder();
      var token = decoder(res.data);
      assert.equal(token.iss, 'ivytech.one');
      assert.equal(token.sub, 'c6dfbcdb25fb444e9f2201083779');
      assert.equal(token.aud, '00000000-0000-0000-0000-000000000001');
    });
  });

  describe('With invalid credentials', function () {
    it('should return 401', async function () {
      const res = await testHelper.makePostRequest('GenerateToken', requestBodyInvlaidCreds);
      console.log('teststatus', res.status);
      assert.equal(res.status, 401, 'Request has 401 status');
    });
  });
});

describe('Anonymous Id', function() {
  describe ('withmissing headers', function () {
    it('get', async function() {
      const res = await testHelper.makeGetRequest('anonymousid', undefined);
      assert.equal(res.status, 400, 'Request has 400 status');
      assert.equal(res.data, 'x-application-id invalid or missing', 'Correct error in response');
    });
  });

  describe('with correct application id', function () {
    it('get', async function() {
      headers = {
        'headers': {
          'x-application-id': '2695BA2C-9C39-4D13-8AC3-B625A0963A19'
        }
      }
  
      const res = await testHelper.makeGetRequest('anonymousid', headers);
      assert.equal(res.status, 200, 'Request has 200 status');
      const decoder = createDecoder();
      var token = decoder(res.data);
      assert.equal(token.iss, 'ivytech.one');
      assert.ok(token.sub.length > 0);
      assert.equal(token.aud, '2695BA2C-9C39-4D13-8AC3-B625A0963A19');
      assert.equal(token.zoneinfo, 'Etc/UTC');
    });
  });

  describe('with correct application id, windows timezone', function () {
    it('get', async function() {
      headers = {
        'headers': {
          'x-application-id': '2695BA2C-9C39-4D13-8AC3-B625A0963A19',
          'x-timezone': 'Pacific Standard Time'
        }
      }
  
      const res = await testHelper.makeGetRequest('anonymousid', headers);
      assert.equal(res.status, 200, 'Request has 200 status');
      const decoder = createDecoder();
      var token = decoder(res.data);
      assert.equal(token.iss, 'ivytech.one');
      assert.ok(token.sub.length > 0);
      assert.equal(token.aud, '2695BA2C-9C39-4D13-8AC3-B625A0963A19');
      assert.equal(token.zoneinfo, 'America/Los_Angeles');
    });
  });

  describe('with correct application id, IANA timezone', function () {
    it('get', async function() {
      headers = {
        'headers': {
          'x-application-id': '2695BA2C-9C39-4D13-8AC3-B625A0963A19',
          'x-timezone': 'Africa/Cairo'
        }
      }
  
      const res = await testHelper.makeGetRequest('anonymousid', headers);
      assert.equal(res.status, 200, 'Request has 200 status');
      const decoder = createDecoder();
      var token = decoder(res.data);
      assert.equal(token.iss, 'ivytech.one');
      assert.ok(token.sub.length > 0);
      assert.equal(token.aud, '2695BA2C-9C39-4D13-8AC3-B625A0963A19');
      assert.equal(token.zoneinfo, 'Africa/Cairo');
    });
  });

  describe('with correct application id, Invalid timezone (empty string)', function () {
    it('get', async function() {
      headers = {
        'headers': {
          'x-application-id': '2695BA2C-9C39-4D13-8AC3-B625A0963A19',
          'x-timezone': ''
        }
      }
  
      const res = await testHelper.makeGetRequest('anonymousid', headers);
      assert.equal(res.status, 400, 'Request has 400 status');
      assert.equal(res.data, 'x-timezone contains invalid time zone', 'correct error message in response');
    });
  });

  describe('with correct application id, Invalid timezone', function () {
    it('get', async function() {
      headers = {
        'headers': {
          'x-application-id': '2695BA2C-9C39-4D13-8AC3-B625A0963A19',
          'x-timezone': 'kfakffadsfas'
        }
      }
  
      const res = await testHelper.makeGetRequest('anonymousid', headers);
      assert.equal(res.status, 400, 'Request has 400 status');
      assert.equal(res.data, 'x-timezone contains invalid time zone', 'correct error message in response');
    });
  });
});

describe('Stop server', function() {
  it('Server stopped', function () {
    assert.ok(testHelper.stopServer());
  });
});