import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AttendanceComponent } from './attendance.component';
import { AttendanceService } from './attendance.service';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AttendanceComponent', () => {
  let component: AttendanceComponent;
  let fixture: ComponentFixture<AttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientTestingModule],
      providers: [AttendanceService],
      declarations: [AttendanceComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty grade and section', () => {
    expect(component.selectedGrade).toBe('');
    expect(component.selectedSection).toBe('');
  });
});
