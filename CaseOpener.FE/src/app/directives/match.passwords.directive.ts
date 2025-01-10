import { Directive } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appMatchPassword]',
  standalone: true,
  providers: [
    {
      provide: NG_VALIDATORS,
      multi: true,
      useExisting: MatchPasswordDirective,
    },
  ],
})
export class MatchPasswordDirective {
  constructor() { }

  validate(control: AbstractControl): ValidationErrors | null{
    const password = control.get('password');
    const confirmPassword = control.get('confirm-password');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordsMismatch: true };
    }

    return null;
  }
}