import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ClassComponent } from './class.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ClassService } from './class.service';
import { FormsModule } from '@angular/forms';

describe('ClassComponent', () => {
  let component: ClassComponent;
  let fixture: ComponentFixture<ClassComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClassComponent],
      imports: [FormsModule, HttpClientTestingModule],
      providers: [ClassService]
    }).compileComponents();

    fixture = TestBed.createComponent(ClassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.selectedGrade).toBe('');
    expect(component.video.subject).toBe('');
    expect(component.presentStudents).toEqual([]);
    expect(component.absentStudents).toEqual([]);
  });
});
