export class Provider {
    providerId: number;
    providerName: string;

    public constructor(
        fields?: {
            providerId: number,
            providerName: string
        }) {
        if (fields)
            Object.assign(this, fields);
    }
}
