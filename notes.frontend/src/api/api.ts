import { ClientBase } from "./client-base";

export interface CreateNoteDto {
    title: string;
    details?: string | null;
}

export interface UpdateNoteDto {
    id: string;
    title?: string | null;
    details?: string | null;
}

export interface NoteLookupDto {
    id: string;
    title?: string | null;
}

export interface NoteListVm {
    notes?: NoteLookupDto[] | null;
}

export interface ProblemDetails {
    type?: string | null;
    title?: string | null;
    status?: number | null;
    detail?: string | null;
    instance?: string | null;
    [key: string]: any;
}

export class Client extends ClientBase {
    private baseUrl: string;
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(); 
        this.http = http || { fetch };
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    protected transformResult(url: string, response: Response, processor: (response: Response) => any): Promise<any> {
        if (!response.ok) {
            return response.text().then(text => {
                let problemDetails: ProblemDetails;
                try {
                    problemDetails = JSON.parse(text);
                } catch {
                    problemDetails = { title: text };
                }
                throw new ApiError(response.status, response.statusText, problemDetails);
            });
        }

        if (response.status === 204) {
            return Promise.resolve(null);
        }

        return processor(response);
    }

    /**
     * GET /api/{version}/Note
     */
    getAll(version: string): Promise<NoteListVm> {
        let url_ = this.baseUrl + "/api/{version}/Note";
        if (version === undefined || version === null)
            throw new Error("The parameter 'version' must be defined.");
        url_ = url_.replace("{version}", encodeURIComponent("" + version));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.trabsformOptions(options_).then((transformedOptions_: RequestInit) => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (response) => response.json());
        });
    }

    /**
     * POST /api/{version}/Note
     */
    create(version: string, createNoteDto: CreateNoteDto): Promise<string> {
        let url_ = this.baseUrl + "/api/{version}/Note";
        if (version === undefined || version === null)
            throw new Error("The parameter 'version' must be defined.");
        url_ = url_.replace("{version}", encodeURIComponent("" + version));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(createNoteDto);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.trabsformOptions(options_).then((transformedOptions_: RequestInit) => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (response) => response.json());
        });
    }

    /**
     * PUT /api/{version}/Note
     */
    update(version: string, updateNoteDto: UpdateNoteDto): Promise<void> {
        let url_ = this.baseUrl + "/api/{version}/Note";
        if (version === undefined || version === null)
            throw new Error("The parameter 'version' must be defined.");
        url_ = url_.replace("{version}", encodeURIComponent("" + version));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(updateNoteDto);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            headers: {}
        };

        return this.trabsformOptions(options_).then((transformedOptions_: RequestInit) => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (response) => Promise.resolve(null));
        });
    }

    /**
     * GET /api/{version}/Note/{id}
     */
    get(version: string, id: string): Promise<NoteListVm> {
        let url_ = this.baseUrl + "/api/{version}/Note/{id}";
        if (version === undefined || version === null)
            throw new Error("The parameter 'version' must be defined.");
        url_ = url_.replace("{version}", encodeURIComponent("" + version));
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.trabsformOptions(options_).then((transformedOptions_: RequestInit) => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (response) => response.json());
        });
    }

    /**
     * DELETE /api/{version}/Note/{id}
     */
    delete(version: string, id: string): Promise<void> {
        let url_ = this.baseUrl + "/api/{version}/Note/{id}";
        if (version === undefined || version === null)
            throw new Error("The parameter 'version' must be defined.");
        url_ = url_.replace("{version}", encodeURIComponent("" + version));
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            headers: {}
        };

        return this.trabsformOptions(options_).then((transformedOptions_: RequestInit) => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (response) => Promise.resolve(null));
        });
    }
}

export class ApiError extends Error {
    constructor(
        public status: number,
        public statusText: string,
        public problemDetails?: ProblemDetails
    ) {
        super(`API Error: ${status} ${statusText}`);
        this.name = 'ApiError';
    }
}