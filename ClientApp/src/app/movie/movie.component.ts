import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Movie } from '../models/movie.type';
import { MovieService } from '../services/movie.service';

@Component({
  selector: 'movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})

export class MovieComponent implements OnInit {
  loading = false;
  public movie: Movie = new Movie();

  constructor(
    private activatedRoute: ActivatedRoute,
    private movieService: MovieService) {
  }

  ngOnInit() {
    let movieId = this.activatedRoute.snapshot.params['id'] as string;
    let provider = this.activatedRoute.snapshot.params['provider'] as string;
    this.getMovie(provider, movieId);
  }

  toggleLoading(isCompleted: boolean) {
    this.loading = !isCompleted;
  }

  getMovie(provider: string, id: string) {
    this.movieService.get(provider, id)
      .subscribe(movie => {
        this.movie = movie;
        this.toggleLoading(true);
      },
        error => {
          console.log(error);
          this.movie = null;
          this.toggleLoading(true);
        });
  }

}
