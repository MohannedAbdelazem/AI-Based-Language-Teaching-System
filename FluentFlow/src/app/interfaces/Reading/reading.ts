export interface ReadingData {
  title: string;
  paragraph: string;
  questions: Question[];
}

interface Question {
  question: string;
  choices: string[];
  rightAnswer: string;
}