export class ClientBase {
    protected trabsformOptions(options: RequestInit){
        const token = localStorage.getItem('token'); 
        options.headers = {
            ...options.headers, 
            Authotization: 'Bearer' + token,
        }; 
        return Promise.resolve(options); 
    }
}