
export interface GrammarData {
  grammar: Grammar[];
}

interface Grammar {
  title: string;
  question: string;
  choices: string[];
  rightAnswer: string;
}