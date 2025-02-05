# Case Opening System Project Documentation

## Description
This project is a **CS:GO Case Opening System** that allows users to open cases and manage their inventory. The system is divided into two main parts:

- **Backend**: Built with ASP.NET Core, providing RESTful APIs for case management, user authentication, and transactions.
- **Frontend**: Developed using Angular, offering an interactive and responsive user interface.

---

## Technologies & Languages

### Technologies 🛠️
- ![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white)
- ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)
- ![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)
- ![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=white)
- ![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=jsonwebtokens) 
- ![NUnit](https://img.shields.io/badge/NUnit-512BD4?style=for-the-badge&logo=.net&logoColor=white)
- ![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)

### Languages 🌍
- ![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
- ![SQL](https://img.shields.io/badge/SQL-4479A1?style=for-the-badge&logo=postgresql&logoColor=white)
- ![HTML](https://img.shields.io/badge/HTML-E34F26?style=for-the-badge&logo=html5&logoColor=white)
- ![CSS](https://img.shields.io/badge/CSS-1572B6?style=for-the-badge&logo=css3&logoColor=white)
- ![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)

---

## Branches 📂
- `main` - Stable production-ready code.
- `develop` - Active development branch.

---

## Libraries 📚
- **Backend:**
  - `Swashbuckle.AspNetCore`
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.Design`
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `Microsoft.EntityFrameworkCore.InMemory`
  - `NUnit`
- **Frontend:**
  - `RxJS`

---

## API Endpoints 🖥️

### AdminController
- `GET /api/admin/users` - Gets all users
- `GET /api/admin/user-transactions` - Gets the user's transactions
- `GET /api/admin/roles` - Gets the existing roles
- `POST /api/admin/add-role` - Adds a new role
- `PUT /api/admin/update-role` - Updates an existing role
- `DELETE /api/admin/delete-transaction` - Deletes an existing transaction
- `PUT /api/admin/update-transaction` - Updates an existing transaction
- `GET /api/admin/transaction` - Gets a transaction by ID

### CaseController
- `GET /api/case/all` - Gets all cases
- `GET /api/case/search` - Searches cases by name
- `GET /api/case/get-case` - Gets a case by ID
- `POST /api/case/add` - Adds a new case
- `PUT /api/case/update` - Updates an existing case
- `POST /api/case/add-case-item` - Adds a new case item
- `DELETE /api/case/delete` - Deletes an existing case
- `POST /api/case/open` - Opens a case
- `GET /api/case/item-probabilities` - Gets case's item probabilities
- `GET /api/case/user-has-case` - Checks if the user has a case
- `POST /api/case/buy-case` - User buys a case
- `GET /api/case/bought-cases` - Gets user's bought cases
- `GET /api/case/user-opened-cases` - Gets user's opened cases

### CategoryController
- `GET /api/category/all` - Gets all categories
- `GET /api/category/get-category` - Gets a category by ID
- `POST /api/category/create` - Adds a new category
- `PUT /api/category/update` - Updates an existing category

### ItemController
- `GET /api/item/all` - Gets all items
- `POST /api/item/add-item` - Adds a new item
- `PUT /api/item/update-item` - Updates an existing item
- `DELETE /api/item/delete-item` - Deletes an item
- `GET /api/item/get-item` - Gets an item by ID
- `POST /api/item/add-item-inventory` - Adds an item to user's inventory
- `DELETE /api/item/remove-item` - Removes an item from user's inventory
- `GET /api/item/get-inventory-item` - Gets an inventory item
- `GET /api/item/user-items` - Gets user's inventory items

### UserController
- `GET /api/user/get-user` - Gets logged-in user
- `GET /api/user/user-info` - Gets user information
- `GET /api/user/is-admin` - Checks if the user is an admin
- `POST /api/user/login` - Logs in a user
- `POST /api/user/register` - Registers a new user
- `PUT /api/user/update-info` - Updates user information
- `PUT /api/user/update-balance` - Updates user balance

---

## Backend Components

### Service Collection Extension
  ```c#
  public static class ServiceCollectionExtension
  {
      public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config) 
      {
          var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");

          services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

          services.AddScoped<IRepository, Repository>();

          return services;
      }

      public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
      {
          services.AddScoped<IUserService, UserService>();
          services.AddScoped<IAdminService, AdminService>();
          services.AddScoped<ITransactionService, TransactionService>();
          services.AddScoped<ICaseService, CaseService>();
          services.AddScoped<IItemService, ItemService>();
          services.AddScoped<ICategoryService, CategoryService>();

          return services;
      }

      public static IServiceCollection AddApplicationCors(this IServiceCollection services)
      {
          services.AddCors(options =>
          {
              options.AddPolicy("AllowAngular", policy =>
              {
                  policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
              });
          });

          return services;
      }

      public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration config)
      {
          services.AddAuthentication(options =>
          {
              options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "http://localhost:4200",
                      ValidAudience = "http://localhost:4200",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                  };
              });

          services.Configure<JwtSettings>(config.GetSection("Jwt"));

          return services;
      }
  }
  ```

### Opening case algorithm
  ```c#
  public async Task<ItemModel> OpenCaseAsync(int caseId, string userId)
  {
      var caseM = await repository.GetByIdAsync<Case>(caseId);

      if (caseM is null)
          throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

      var user = await repository.GetByIdAsync<User>(userId);

      if (user is null)
          throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

      var caseUserModel = await repository.All<CaseUser>()
          .FirstOrDefaultAsync(x => x.UserId == userId && x.CaseId == caseId);

      if(caseUserModel == null)
          throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Case"));

      if (caseUserModel.Quantity == 1)
          await repository.DeleteAsync<CaseUser>(caseUserModel.Id);
      else
          caseUserModel.Quantity -= 1;

      var items = await GetCaseItemsProbabilities(caseM.Id);

      var itemId = GetRandomItem(items.ToList());

      var item = await repository.GetByIdAsync<Item>(itemId);

      if (item is null)
          throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Item"));

      var caseOpening = new CaseOpening()
      {
          UserId = userId,
          ItemId = itemId,
          CaseId = caseId,
          DateOpened = DateTime.Now,
      };

      await repository.AddAsync(caseOpening);

      await repository.SaveChangesAsync();

      return new ItemModel() 
      {
          Id = item.Id,
          Name = item.Name,
          ImageUrl = item.ImageUrl,
          Rarity = item.Rarity,
          Type = item.Type,
          Amount = item.Amount,
      };
  }

  private int GetRandomItem(List<CaseItemModel> items)
  {
      var cumulativeProbability = 0.0;
      var weightedItems = items
          .Select(item => new
          {
              Item = item,
              CumulativeProbability = cumulativeProbability += item.Probability
          })
          .ToList();

      var random = new Random();
      var randomValue = random.NextDouble() * cumulativeProbability;

      return weightedItems.First(w => randomValue <= w.CumulativeProbability).Item.Id;
  }
  ```

---

## SQL Queries

### Seed 33 Cases, example: 
  ```sql
  INSERT INTO [Cases] ([Name], Price, CategoryId, ImageUrl)
  VALUES
  ('Recoil', 0.3, 1, 'https://community.fastly.steamstatic.com/economy/image/-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXU5A1PIYQNqhpOSV-fRPasw8rsUFJ5KBFZv668FFQxnaecIT8Wv9rilYTYkfTyNuiFwmhUvpZz3-2Z9oqg0Vew80NvZzuiJdeLMlhpwFO-XdA/360fx360f'),
  ```

### Seed 5 Categories, example:
  ```sql
  INSERT INTO Categories ([Name])
  VALUES
  ('Weapon'),
  ```

### Seed 553 Items, example:
  ```sql
  INSERT INTO Items ([Name], [Type], Rarity, Amount, ImageUrl)
  VALUES  
  ('SG 553 | Heavy Metal', 'Skin', 'MilSpec', 0.64, 'https://community.fastly.steamstatic.com/economy/image/-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXH5ApeO4YmlhxYQknCRvCo04DEVlxkKgpopb3wflFf1OD3YjoXuY-JkIWKg__5Nq_QmlRd4cJ5nqeS9tWs2wXiqBVvZmqlLYGccVNtYFzS_FTtxr_shp68usnOmyBgvXYn-z-DyNhs9kJY/360fx360f'),
  ```

### Seed 552 Mapping Case Items, example:
  ```sql
  INSERT INTO CaseItems (CaseId, ItemId, Probability)
  VALUES
  (1, 1, 11.97),  
  ```

---

## Frontend Components

### Email Directive
  ```typescript
  export class EmailDirective {
      constructor() { }

      validate(control: AbstractControl): ValidationErrors | null{
          const validatorFn = emailValidator();

          return validatorFn(control);
      }   
  }
  ```

### Email Validator
  ```typescript
  export function emailValidator(): ValidatorFn {
      const regExp = new RegExp(`^[A-Za-z0-9]{2,}@gmail\.com$`);

      return (control) => {
          const isInvalid = control.value === '' || regExp.test(control.value);
          return isInvalid ? null : { emailValidator: true };
      };
  }
  ```

### Match Password Directive
  ```typescript
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
  ```

### Authentication Guard
  ```typescript
  export const AuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
      const userService = inject(UserService);
      const router = inject(Router);
  
      if (userService.isLoggedIn()) {
      return true;
      }

      router.navigate(['/login']);
      return false;
  };
  ```

### Slice Pipe
  ```typescript
  export class SlicePipe implements PipeTransform {
      transform(value: string, maxCharCount = 5): unknown {
          const dots = value.length > maxCharCount ? '...' : '';
          return `${value.substring(0, maxCharCount)}${dots}`;
      }
  }
  ```

### App Interceptor
  ```typescript
  const { apiUrl } = environment;
  const API = '/api';

  export const appInterceptor: HttpInterceptorFn = (req, next) => {
      if (req.url.startsWith(API)) {
          req = req.clone({
          url: req.url.replace(API, apiUrl),
          withCredentials: true,
          });
      }
      
      return next(req);
  };
  ```

### App Routing
  ```typescript
  export const routes: Routes = [
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'home', component: MainComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'history', loadComponent: () =>
          import('./case/opened-cases/opened-cases.component').then(
          (c) => c.OpenedCasesComponent), canActivate: [AuthGuard],
      },
      { path: 'settings', loadComponent: () =>
          import('./user/profile/profile.component').then(
          (c) => c.ProfileComponent), canActivate: [AuthGuard],
      },
      { path: 'inventory', loadComponent: () =>
          import('./inventory/inventory.component').then(
          (c) => c.InventoryComponent), canActivate: [AuthGuard],
      },
      {
          path: 'case',
          children: [
          {
              path: ':caseId',
              loadComponent: () =>
                  import('./case/case-details/case-details.component').then((c) => c.CaseDetailsComponent)
          },
          ],
      },
      { path: 'mycases', loadComponent: () =>
          import('./case/bought-cases/bought-cases.component').then(
          (c) => c.BoughtCasesComponent), canActivate: [AuthGuard],
      }, 
      { path: '404', component: ErrorComponent },
      { path: '**', redirectTo: '/404' },
  ];
  ```

### Responsive Design
  ```css
  @media only screen and (max-width: 1200px) {
  }
  ```

---

## Installation & Setup 🛠️
1. Clone the repository:
   ```sh
   git clone https://github.com/nikolaymihalev/Case-Opening-System.git
   ```
2. Navigate to the CaseOpener.API folder and restore dependencies:
   ```sh
   dotnet restore
   ```
3. Navigate to the CaseOpener.Infrastructure folder and apply migrations:
   ```sh
   dotnet ef update-database
   ```
3. Navigate to the CaseOpener.API folder and start the backend server:
   ```sh
   dotnet run
   ```
4. Navigate to the CaseOpener.FE folder and install dependencies:
   ```sh
   npm install
   ```
5. Start the frontend application:
   ```sh
   ng serve
   ```
6. Open the application in your browser at `http://localhost:4200`

---

## Running Unit Tests 🛠️
- **To execute unit tests, navigate to the CaseOpener.UnitTests directory and run:**
    ```sh
    dotnet test
    ```

---

## License 📜
This project is licensed under the **MIT License**.
