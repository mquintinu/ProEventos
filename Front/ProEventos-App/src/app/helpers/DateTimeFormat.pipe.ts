import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateFormatPipe'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  override transform(value: any, args?: any): any {
    const dateValue = new Date(value);
    if (!isNaN(dateValue.getTime())) {
      return super.transform(dateValue, Constants.DATE_TIME_FMT);
    }
    return value;
  }
}
