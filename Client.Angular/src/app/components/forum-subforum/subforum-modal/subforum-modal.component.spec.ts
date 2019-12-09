import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubforumModalComponent } from './subforum-modal.component';

describe('SubforumModalComponent', () => {
  let component: SubforumModalComponent;
  let fixture: ComponentFixture<SubforumModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubforumModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubforumModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
