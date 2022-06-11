import { NDDSApiService } from '../src'
const keyUrl = 'http://localhost:8080/auth/realms/fakedds/protocol/openid-connect/token'
const gateUrl = 'http://localhost/api'

const nddsSvc = new NDDSApiService(keyUrl, gateUrl)

beforeAll( async () => {
    await nddsSvc.init() // Must Init to Get the AuthN
  });

test('Got an AuthN Token', () => {
    const tok = nddsSvc.authTok
    console.log('token =>', tok)
    expect(nddsSvc.authTok).toBeTruthy();
});

test('Get Userprofile Test', async () => {
  const uprof = await nddsSvc.GetUserProfile('9111111111')

  console.log('prof =>', uprof)
  expect(uprof).toBeTruthy();
});
  