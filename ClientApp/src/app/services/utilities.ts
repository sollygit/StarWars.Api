import { Injectable } from '@angular/core';

@Injectable()
export class Utilities {

  public static randomNumber(min: number, max: number) {
    return Math.floor(Math.random() * (max - min + 1) + min);
  }

  public static parseDate(date: any) {

    if (date) {

      if (date instanceof Date) {
        return date;
      }

      if (typeof date === 'string' || date instanceof String) {
        if (date.search(/[a-su-z+]/i) == -1)
          date = date + "Z";

        return new Date(date);
      }

      if (typeof date === 'number' || date instanceof Number) {
        return new Date(<any>date);
      }
    }
  }

  public static randomDate(start: Date, end: Date): Date {
    return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
  }

}
