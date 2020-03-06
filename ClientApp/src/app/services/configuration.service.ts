import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable()
export class ConfigurationService {
  public static readonly appVersion: string = "1.0.1";
  public baseUrl = environment.baseUrl;
  public cinemaWorld = environment.cinemaWorld;
  public filmWorld = environment.filmWorld;

  private _dateFormat: string = 'dd-MM-yyyy';
  private _providers = [
    { value: this.cinemaWorld, display: this.cinemaWorld },
    { value: this.filmWorld, display: this.cinemaWorld }
  ];

  constructor() { }

  get dateFormat() {
    return this._dateFormat;
  }

  get providers() {
    return this._providers;
  }

}
