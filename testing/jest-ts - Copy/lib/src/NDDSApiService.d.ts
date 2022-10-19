export declare class NDDSApiService {
    private keyUrl;
    private gwUrl;
    authTok: string | undefined;
    constructor(keycloak: string, gateway: string);
    init: () => Promise<void>;
    GetUserProfile: (edipi: string) => Promise<any>;
}
