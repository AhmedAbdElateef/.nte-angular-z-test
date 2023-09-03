import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditshwopostComponent } from './editshwopost.component';

describe('EditshwopostComponent', () => {
  let component: EditshwopostComponent;
  let fixture: ComponentFixture<EditshwopostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditshwopostComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditshwopostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
