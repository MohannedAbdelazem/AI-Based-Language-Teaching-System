import { Component, ElementRef, ViewChild, EventEmitter, Output, AfterViewInit, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { QuestionsService } from '../../../services/Questions/questions.service';
import { ListeningData } from '../../../interfaces/Listening/listening';

@Component({
  selector: 'app-listening-questions',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './listening-questions.component.html',
  styleUrls: ['./listening-questions.component.scss']
})
export class ListeningQuestionsComponent implements OnInit{

  questions: ListeningData[] = [];
  listening: string = 'listening';
  constructor(private _QuestionsService:QuestionsService) { }
  ngOnInit(): void {
    if(this._QuestionsService.listeningQuestions.getValue() !== null)
    {
        this.questions = this._QuestionsService.listeningQuestions.getValue().listening;
        return;
    }
    this._QuestionsService.getListeningQuestions().subscribe((data) => {
      this.questions = data.listening;
    });
  }
  @ViewChild('audioElement') audioElement!: ElementRef<HTMLAudioElement>;

  isPlaying = false;
  duration = 0;
  currentTime = 0;
  progressPercentage = 0;
  audio: HTMLAudioElement | null = null;


  currentTrackIndex = 0;

  get currentTrack() {
    return this.questions[this.currentTrackIndex];
  }

  togglePlay(): void {
    if (!this.audioElement) {
      console.error('audioElement is not initialized');
      return;
    }
    const audio = this.audioElement.nativeElement;
    if (!audio.src) {
    this.loadCurrentTrack();
    }
    if (this.isPlaying) {
      audio.pause();
      this.isPlaying = false;
    } else {
      audio.play();
      this.isPlaying = true;
    }
    
  }

  updateProgress(): void {
    if (!this.audioElement) {
      console.error('audioElement is not initialized');
      return;
    }
    const audio = this.audioElement.nativeElement;
    if (audio.duration) {
      this.currentTime = audio.currentTime;
      this.duration = audio.duration;
      this.progressPercentage = (audio.currentTime / audio.duration) * 100;
    }
  }

  setProgress(event: MouseEvent): void {
    const audio = this.audioElement.nativeElement;
    const progressContainer = event.currentTarget as HTMLElement;
    const rect = progressContainer.getBoundingClientRect();
    const clickX = event.clientX - rect.left;
    const width = rect.width;
    const newTime = (clickX / width) * audio.duration;
    audio.currentTime = newTime;
  }

  loadCurrentTrack(): void {
    const audio = this.audioElement.nativeElement;
    audio.src = this.currentTrack.audioUrl;
    audio.load();
  }

  formatTime(seconds: number): string {
    const mins = Math.floor(seconds / 60);
    const secs = Math.floor(seconds % 60);
    return mins.toString().padStart(2, '0') + ':' + secs.toString().padStart(2, '0');
  }
}
