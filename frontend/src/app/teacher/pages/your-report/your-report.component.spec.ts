import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YourReportComponent } from './your-report.component';

describe('YourReportComponent', () => {
  let component: YourReportComponent;
  let fixture: ComponentFixture<YourReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YourReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YourReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
