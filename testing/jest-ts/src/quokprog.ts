import { NDDSApiService } from '../src'
const keyUrl = 'http://localhost:8080/auth/realms/fakedds/protocol/openid-connect/token'
const gateUrl = 'http://localhost/api';


(async () => {
    const nddsSvc = new NDDSApiService(keyUrl, gateUrl)

    await nddsSvc.init()

    var res = await nddsSvc.GetUserProfile('9111111111')

    console.log(res)

})
