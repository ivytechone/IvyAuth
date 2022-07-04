console.log("IvyAuthTest Starting");

const artifactDir = "";

//const {spawn} = require('node:child_process');
const axios = require('axios');
const { createDecoder } = require('fast-jwt')


//const server = spawn()

var requestBody = {
    userName: 'testuser1',
    passWord: 'appleOrange34'
};

// copy settings

// launch service

// verify JWT

function VerifySuccess() {

axios.post('http://localhost:5000/api/GenerateToken', requestBody)
    .then((res) => {
        if (res.status != 200) {

        }

        const decode = createDecoder();
        const token = decode(res.data);

        console.log(token);
    });
}

VerifySuccess();
