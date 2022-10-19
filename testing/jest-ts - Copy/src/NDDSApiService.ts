import agnt from 'superagent'

export class NDDSApiService {
    private keyUrl: string
    private gwUrl: string
    public authTok: string | undefined 
    constructor(keycloak: string, gateway: string) {
        this.keyUrl = keycloak  
        this.gwUrl = gateway
    }

    public init = async () => {
        try {
            let creds = { grant_type: 'password', client_id: 'testapp', username: 'test1', password: 'test1'};
            const res = await agnt.post(this.keyUrl)
                .set('Content-Type', 'application/x-www-form-urlencoded')
                .send(creds) // sends a JSON with the user creds
            this.authTok = res.body['access_token']
        } catch (error) {
            console.log(error.response.body);
        }
    }

    public GetUserProfile = async (edipi:string) => {
        const res = await agnt.get(`${this.gwUrl}/UserProfile/${edipi}`)
            .set('Content-Type', 'application/json')
            .set('Authorization', `bearer ${this.authTok}`)

         return res.body
    }


}

