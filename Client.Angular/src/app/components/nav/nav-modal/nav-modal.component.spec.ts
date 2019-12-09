import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavModalComponent } from './nav-modal.component';

describe('NavModalComponent', () => {
  let component: NavModalComponent;
  let fixture: ComponentFixture<NavModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
