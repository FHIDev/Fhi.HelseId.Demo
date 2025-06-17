import { Component, Input } from '@angular/core';
import { FhiPopoverMenuComponent } from '@folkehelseinstituttet/angular-components';
import { AuthenticationService } from '../authentication.service';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    imports: [FhiPopoverMenuComponent]
})

export class NavMenuComponent {
  authService:AuthenticationService;

  constructor(auth: AuthenticationService){
    this.authService = auth;
  }

  popoverMenuItems = [
    {
      icon: 'plus',
      name: 'Log in',
      link: {
        href: 'Account/Login',
      }
    },
    {
      icon: '',
      name: 'Log out',
      link: {
        href: 'Account/Logout',
      }
    },
    
  ];

  action(action: string) {
  
  }


}
