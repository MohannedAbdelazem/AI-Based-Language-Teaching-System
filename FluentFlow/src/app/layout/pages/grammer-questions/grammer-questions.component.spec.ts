import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GrammerQuestionsComponent } from './grammer-questions.component';

describe('GrammerQuestionsComponent', () => {
  let component: GrammerQuestionsComponent;
  let fixture: ComponentFixture<GrammerQuestionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GrammerQuestionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GrammerQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
