export interface ReadingData {
  paragraphTitle: string;
  paragraph: string;
  questions: Question[];
}

interface Question {
  question: string;
  choices: string[];
  rightAnswer: string;
}