export interface ListeningData {
  listening: Listening[];
}

interface Listening {
  title: string;
  audioUrl: string;
  question: string;
  choices: string[];
  rightAnswer: string;
}