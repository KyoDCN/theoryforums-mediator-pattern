import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumModalComponent } from './forum-modal.component';

describe('ForumModalComponent', () => {
  let component: ForumModalComponent;
  let fixture: ComponentFixture<ForumModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForumModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForumModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
