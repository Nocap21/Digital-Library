import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentService } from '../../services/student.service';
import { AddStudentComponent } from './add-student.component';

describe('AddStudentComponent', () => {
  let fixture: ComponentFixture<AddStudentComponent>;
  let studentService: StudentService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [AddStudentComponent] }).compileComponents();
    fixture = TestBed.createComponent(AddStudentComponent);
    studentService = TestBed.inject(StudentService);
    fixture.detectChanges();
  });

  it('creates the page component', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('requires Roll No., Name, and Branch', () => {
    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    fixture.detectChanges();

    expect(studentService.getStudents()).toEqual([]);
    expect(fixture.nativeElement.textContent).toContain('Roll No. is required.');
    expect(fixture.nativeElement.textContent).toContain('Name is required.');
    expect(fixture.nativeElement.textContent).toContain('Branch is required.');
  });

  it('offers exactly IT, MECH, and CIVIL and saves each valid branch through StudentService', () => {
    const branches = ['IT', 'MECH', 'CIVIL'];
    const branchSelect = fixture.nativeElement.querySelector('#branch') as HTMLSelectElement;
    const options = Array.from(branchSelect.options)
      .filter((option) => option.value !== '')
      .map((option) => option.value);
    expect(options).toEqual(branches);

    for (const [index, branch] of branches.entries()) {
      const rollNo = fixture.nativeElement.querySelector('#rollNo') as HTMLInputElement;
      const name = fixture.nativeElement.querySelector('#name') as HTMLInputElement;
      rollNo.value = `100${index + 1}`;
      rollNo.dispatchEvent(new Event('input', { bubbles: true }));
      name.value = `Student ${index + 1}`;
      name.dispatchEvent(new Event('input', { bubbles: true }));
      branchSelect.value = branch;
      branchSelect.dispatchEvent(new Event('change', { bubbles: true }));
      fixture.detectChanges();

      const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      fixture.detectChanges();
    }

    expect(studentService.getStudents()).toEqual([
      { rollNo: '1001', name: 'Student 1', branch: 'IT' },
      { rollNo: '1002', name: 'Student 2', branch: 'MECH' },
      { rollNo: '1003', name: 'Student 3', branch: 'CIVIL' }
    ]);
  });
});
