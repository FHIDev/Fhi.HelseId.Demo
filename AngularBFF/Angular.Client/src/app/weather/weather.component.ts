import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { map, catchError, of } from 'rxjs';

interface WeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
  }

@Component({
    selector: 'weather-component',
    templateUrl: 'weather.component.html',
    imports: [NgIf, NgFor]
})

export class WeatherForecastComponent implements OnInit{
    constructor(private http: HttpClient) {}
    public forecasts: WeatherForecast[] = [];

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/api/v1/weatherforecast').pipe(
      map(result => {
        return result;
      }),
      catchError(error => {
        console.error(error);
        return of([]);
      })
      ).subscribe(result => {
        this.forecasts = result;
      });
      
  }
}
