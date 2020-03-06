export class Movie {
  id: string;
  title: string;
  year: string;
  type: string;
  poster: string;
  price: number;

  public constructor(
    fields?: {
      id?: string,
      title?: string,
      year?: string,
      type?: string,
      poster?: string,
      price?: number
    }) {
    if (fields) {
      Object.assign(this, fields);
    }
  }
}
