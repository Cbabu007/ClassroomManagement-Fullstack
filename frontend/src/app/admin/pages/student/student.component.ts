import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { StudentService } from './student.service'; // ✅ Import your service

@Component({
  selector: 'app-student',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent {
  constructor(private studentService: StudentService) {}

  activeTab = 'add'; // Which tab is active
  searchStudentId: string = ''; // Student ID input for View/Edit/Delete
  photoPreviewUrl: string | ArrayBuffer | null = null; // Photo preview

  grades = ['1st', '2nd', '3rd', '4th', '5th', '6th', '7th', '8th', '9th', '10th', '11th', '12th'];
  academicYears = ['2023-2024', '2024-2025', '2025-2026'];

  student: any = {
    studentId: '', firstName: '', lastName: '', dob: '', gender: '', bloodGroup: '', aadhar: '',
    mobile: '', altMobile: '', email: '', doorNo: '', address1: '', address2: '', city: '', district: '',
    state: '', pincode: '', country: '', admissionNo: '', admissionDate: '', grade: '', section: '',
    rollNo: '', medium: '', academicYear: '', entranceTest: '', fatherName: '', fatherJob: '', fatherMobile: '',
    motherName: '', motherJob: '', motherMobile: '', guardianName: '', guardianRelation: '', guardianMobile: '',
    photo: null, prevSchool: '', tc: null, idProof: null, addressProof: null,   communityType: '', communityName: '', religion: '', status: 'Active', reasonLeaving: ''
  };

  religions = [
    'Hinduism', 'Islam', 'Christianity', 'Sikhism',
    'Buddhism', 'Jainism', 'Judaism', 'Zoroastrianism (Parsis)',
    'Bahá\'í Faith', 'Other Tribal and Indigenous Religions'
  ];
  
  // Tab switching
  setTab(tab: string) {
    this.activeTab = tab;
    this.resetForm();
  }

  // Reset Form
  resetForm() {
    this.student = {
      studentId: '', firstName: '', lastName: '', dob: '', gender: '', bloodGroup: '', aadhar: '',
      mobile: '', altMobile: '', email: '', doorNo: '', address1: '', address2: '', city: '', district: '',
      state: '', pincode: '', country: '', admissionNo: '', admissionDate: '', grade: '', section: '',
      rollNo: '', medium: '', academicYear: '', fatherName: '', fatherJob: '', fatherMobile: '',
      motherName: '', motherJob: '', motherMobile: '', guardianName: '', guardianRelation: '', guardianMobile: '',
      photo: null, prevSchool: '', tc: null, idProof: null, addressProof: null,    communityType: '', communityName: '', religion: '', status: 'Active', reasonLeaving: ''
    };
    this.photoPreviewUrl = null;
    this.searchStudentId = '';
  }

  // Preview uploaded photo
previewPhoto(event: any) {
  const file = event.target.files[0];
  if (file) {
    this.student.photo = file;
    const reader = new FileReader();
    reader.onload = () => {
      this.student.photoPath = reader.result as string;
    };
    reader.readAsDataURL(file);
  }
}


  // Upload other files (tc, idProof, addressProof)
  uploadFile(event: any, type: string) {
    const file = event.target.files[0];
    if (file) {
      this.student[type] = file;
    }
  }

  // Submit new student
  submitForm() {
    const formData = new FormData();
    for (const key in this.student) {
      formData.append(key, this.student[key]);
    }

    this.studentService.addStudent(formData).subscribe({
      next: res => {
        alert('✅ Student saved successfully!');
        this.resetForm();
      },
      error: err => {
        console.error('❌ Save Error:', err);
        alert('Failed to save student.');
      }
    });
  }

  // Fetch student for edit/view/delete
  fetchStudent() {
    if (!this.searchStudentId.trim()) {
      alert('Please enter Student ID');
      return;
    }

    this.studentService.getStudentById(this.searchStudentId).subscribe({
      next: (res) => {
        this.student = { ...res }; // ✅ full student details fill
        
        if (res.photoPath) {
          const safePhotoPath = res.photoPath.startsWith('http')
            ? res.photoPath
            : 'https://localhost:7284/' + res.photoPath;
  
          this.student.photoPath = encodeURI(safePhotoPath); // ✅ fix student.photoPath also
          this.photoPreviewUrl = encodeURI(safePhotoPath);   // ✅ fix preview separately
        }
        this.student.tcPath = res.tcPath?.startsWith('http')
  ? res.tcPath
  : 'https://localhost:7284/' + res.tcPath;

this.student.idProofPath = res.idProofPath?.startsWith('http')
  ? res.idProofPath
  : 'https://localhost:7284/' + res.idProofPath;

this.student.addressProofPath = res.addressProofPath?.startsWith('http')
  ? res.addressProofPath
  : 'https://localhost:7284/' + res.addressProofPath;
      },
      error: () => {
        alert('❌ Student not found');
        this.resetForm();
      }
    });
  }

  // Update existing student
  updateStudent() {
    const formData = new FormData();
    for (const key in this.student) {
      formData.append(key, this.student[key]);
    }

    this.studentService.updateStudent(this.searchStudentId, formData).subscribe({
      next: () => {
        alert('✅ Student updated successfully!');
        this.resetForm();
      },
      error: () => {
        alert('❌ Update failed');
      }
    });
  }

  // Delete student
  deleteStudent() {
    if (!confirm('Are you sure you want to delete this student?')) return;

    this.studentService.deleteStudent(this.searchStudentId).subscribe({
      next: () => {
        alert('🗑️ Student deleted successfully!');
        this.resetForm();
      },
      error: () => {
        alert('❌ Delete failed');
      }
    });
  }
}
