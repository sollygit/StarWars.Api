import { Component, OnInit } from '@angular/core';
import { Movie } from '../models/movie.type';
import { Provider } from '../models/provider.type';
import { ConfigurationService } from '../services/configuration.service';
import { MovieService } from '../services/movie.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css']
})
export class FetchDataComponent implements OnInit {
  loading = false;
  public movies: Movie[] = [];
  public providerList: Provider[] = [];

  constructor(private movieService: MovieService,
    private config: ConfigurationService) {
    this.providerList = this.movieService.providers;
  }

  ngOnInit(): void {
    this.getMovies(this.config.cinemaWorld);
  }

  toggleLoading(isCompleted: boolean) {
    this.loading = !isCompleted;
  }

  getMovies(provider: string) {
    this.movieService.getAll(provider)
      .subscribe(movies => {
        movies.map(m => m.id = m.id.toUpperCase());
        this.movies = movies;
        this.toggleLoading(true);
      },
        error => {
          console.log(error);
          this.movies = [];
          this.toggleLoading(true);
        });
  }

}
