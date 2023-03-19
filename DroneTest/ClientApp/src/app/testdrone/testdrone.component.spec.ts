import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestDroneComponent } from './testdrone.component';

describe('TestDroneComponent', () => {
  let fixture: ComponentFixture<TestDroneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TestDroneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestDroneComponent);
    fixture.detectChanges();
  });

});
