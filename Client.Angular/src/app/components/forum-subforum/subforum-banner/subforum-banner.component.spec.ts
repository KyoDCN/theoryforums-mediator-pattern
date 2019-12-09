import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubforumBannerComponent } from './subforum-banner.component';

describe('SubforumBannerComponent', () => {
  let component: SubforumBannerComponent;
  let fixture: ComponentFixture<SubforumBannerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubforumBannerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubforumBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
