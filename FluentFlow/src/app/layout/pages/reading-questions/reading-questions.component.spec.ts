import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingQuestionsComponent } from './reading-questions.component';

describe('ReadingQuestionsComponent', () => {
  let component: ReadingQuestionsComponent;
  let fixture: ComponentFixture<ReadingQuestionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReadingQuestionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReadingQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
