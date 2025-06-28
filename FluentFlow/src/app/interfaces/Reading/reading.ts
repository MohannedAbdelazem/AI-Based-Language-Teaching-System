export interface ReadingData {
  reading: Reading[];
}

interface Reading {
  paragraphTitle: string;
  paragraph: string;
  question: string;
  choices: string[];
  rightAnswer: string;
}