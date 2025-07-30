import { Component, inject } from '@angular/core'; // You also forgot to import `inject`
import { RouterOutlet } from '@angular/router';
import { WeatherforecastService } from './weatherforecast.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'] // ✅ Fixed
})
export class AppComponent {
  title = 'demo';
  weatherForecasts: any[] = [];

  weatherForecastService = inject(WeatherforecastService);

  constructor() {
    this.weatherForecastService.get().subscribe(weatherForecasts => {
      this.weatherForecasts = weatherForecasts;
    });
  }
}
