import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListeningQuestionsComponent } from './listening-questions.component';

describe('ListeningQuestionsComponent', () => {
  let component: ListeningQuestionsComponent;
  let fixture: ComponentFixture<ListeningQuestionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListeningQuestionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListeningQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
