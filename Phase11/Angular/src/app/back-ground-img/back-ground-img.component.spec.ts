import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackGroundImgComponent } from './back-ground-img.component';

describe('BackGroundImgComponent', () => {
  let component: BackGroundImgComponent;
  let fixture: ComponentFixture<BackGroundImgComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BackGroundImgComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BackGroundImgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
