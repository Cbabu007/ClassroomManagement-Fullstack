// 🛡️ staff.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StaffService } from './staff.service';

@Component({
  selector: 'app-staff',
  standalone: true,
  templateUrl: './staff.component.html',
  styleUrls: ['./staff.component.css'],
  imports: [CommonModule, FormsModule]
})
export class StaffComponent {
  activeTab: string = 'add';
  staff: Staff = new Staff();
  searchStaffId: string = '';
  newQualification: Qualification = new Qualification();
  photoPreviewUrl: string = '';

  departments: string[] = ['English', 'Maths', 'Science', 'Social', 'Tamil', 'Hindi', 'Computer Science', 'Transport', 'Maintenance', 'Driver', 'Cleaner', 'Office Staff'];
  roles: string[] = ['Teacher', 'Principal', 'Vice Principal', 'Admin', 'Accountant', 'Driver', 'Cleaner', 'Office Assistant'];

  constructor(private staffService: StaffService) { }

  setTab(tab: string) {
    this.activeTab = tab;
    this.clearForm();
  }

  clearForm() {
    this.staff = new Staff();
    this.newQualification = new Qualification();
    this.searchStaffId = '';
    this.photoPreviewUrl = '';
  }

  previewPhoto(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.photoPreviewUrl = e.target.result;
      };
      reader.readAsDataURL(file);
      this.staff.photo = file;
    }
  }

  addQualification() {
    if (!this.staff.qualifications) {
      this.staff.qualifications = [];
    }
    this.staff.qualifications.push({ ...this.newQualification });
    this.newQualification = new Qualification();
  }

  removeQualification(index: number) {
    this.staff.qualifications.splice(index, 1);
  }

  prepareFormData(): FormData {
    const formData = new FormData();
    for (const key in this.staff) {
      if (key !== 'qualifications' && key !== 'photo') {
        formData.append(key, (this.staff as any)[key] || '');
      }
    }

    if (this.staff.photo) {
      formData.append('photo', this.staff.photo);
    }

    if (this.staff.qualifications && this.staff.qualifications.length > 0) {
      this.staff.qualifications.forEach((q, index) => {
        formData.append(`qualifications[${index}].degree`, q.degree);
        formData.append(`qualifications[${index}].specialization`, q.specialization);
        formData.append(`qualifications[${index}].institution`, q.institution);
        formData.append(`qualifications[${index}].university`, q.university);
        formData.append(`qualifications[${index}].yearOfPassing`, q.yearOfPassing);
      });
    }
    return formData;
  }

  submitStaff() {
    const formData = this.prepareFormData();
    this.staffService.addStaff(formData).subscribe({
      next: (res) => {
        alert('Staff added successfully!');
        this.clearForm();
      },
      error: (err) => {
        alert('Failed to add staff.');
        console.error(err);
      }
    });
  }

  fetchStaff() {
    if (!this.searchStaffId) {
      alert('Please enter Staff ID.');
      return;
    }
    this.staffService.getStaffById(this.searchStaffId).subscribe({
      next: (res: any) => {
        this.staff = res.staff;
        this.staff.qualifications = res.qualifications || [];

        if (res.staff.photoPath) {
          this.photoPreviewUrl = res.staff.photoPath.startsWith('http')
            ? res.staff.photoPath
            : 'https://localhost:7284/' + res.staff.photoPath;
        }
      },
      error: (err) => {
        alert('Staff not found.');
        console.error(err);
      }
    });
  }

  updateStaff() {
    if (!this.staff.staffId) {
      alert('Please search and load staff details first.');
      return;
    }
    const formData = this.prepareFormData();
    this.staffService.updateStaff(this.staff.staffId, formData).subscribe({
      next: (res: any) => {
        alert('Staff updated successfully!');
        this.clearForm();
      },
      error: (err) => {
        alert('Failed to update staff.');
        console.error(err);
      }
    });
  }

  fetchStaffForDelete() {
    if (!this.searchStaffId) {
      alert('Please enter Staff ID.');
      return;
    }
    this.staffService.getStaffForDelete(this.searchStaffId).subscribe({
      next: (res: any) => {
        this.staff = res;
        if (res.photoPath) {
          this.photoPreviewUrl = res.photoPath.startsWith('http')
            ? res.photoPath
            : 'https://localhost:7284/' + res.photoPath;
        }
      },
      error: (err) => {
        alert('Staff not found.');
        console.error(err);
      }
    });
  }

  deleteStaff() {
    if (!this.searchStaffId) {
      alert('Please enter Staff ID to delete.');
      return;
    }
    if (confirm('Are you sure you want to delete this staff?')) {
      this.staffService.deleteStaff(this.searchStaffId).subscribe({
        next: (res) => {
          alert('Staff deleted successfully!');
          this.clearForm();
        },
        error: (err) => {
          alert('Failed to delete staff.');
          console.error(err);
        }
      });
    }
  }
  fetchStaffForView() {
    if (!this.searchStaffId) {
      alert('Please enter Staff ID.');
      return;
    }
    this.staffService.getStaffById(this.searchStaffId).subscribe({
      next: (res: any) => {
        this.staff = res.staff;
        if (res.staff.photoPath) {
          this.photoPreviewUrl = res.staff.photoPath.startsWith('http')
            ? res.staff.photoPath
            : 'https://localhost:7284/' + res.staff.photoPath;
        }
        if (res.qualifications) {
          this.staff.qualifications = res.qualifications;
        } else {
          this.staff.qualifications = [];
        }
      },
      error: (err) => {
        alert('Staff not found.');
        console.error(err);
      }
    });
  }
  

}

// Model Classes
export class Staff {
  staffId: string = '';
  firstName: string = '';
  lastName: string = '';
  gender: string = '';
  dateOfBirth: string = '';
  bloodGroup: string = '';
  aadharNumber: string = '';
  mobile: string = '';
  altMobile: string = '';
  email: string = '';
  doorNo: string = '';
  address1: string = '';
  address2: string = '';
  taluk: string = '';
  district: string = '';
  state: string = '';
  pincode: string = '';
  country: string = '';
  department: string = '';
  role: string = '';
  joiningDate: string = '';
  staffType: string = '';
  bankName: string = '';
  accountNumber: string = '';
  ifscCode: string = '';
  panCardNumber: string = '';
  username: string = '';
  password: string = '';
  loginRole: string = '';
  photoPath: string = '';
  photo: any;
  qualifications: Qualification[] = [];
}

export class Qualification {
  degree: string = '';
  specialization: string = '';
  institution: string = '';
  university: string = '';
  yearOfPassing: string = '';
}
