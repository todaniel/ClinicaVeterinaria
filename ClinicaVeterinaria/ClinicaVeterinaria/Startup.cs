﻿using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;

namespace ClinicaVeterinaria
{
    public partial class Startup{

        public void Configuration(IAppBuilder app){
            ConfigureAuth(app);

            //método para inicializar os dados na BD
            //em particular:
            // - Os Roles e
            // - Utilizadores

            IniciaAplicacao();
        }

        

        /// <summary>
        /// cria, caso não existam, as Roles de suporte à aplicação: Veterinario, Funcionario, Dono
        /// cria, nesse caso, também, um utilizador...
        /// </summary>
        private void IniciaAplicacao(){

            VetsDB db = new VetsDB();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // criar a Role 'Veterinario'
            if (!roleManager.RoleExists("Veterinario")){
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Veterinario";
                roleManager.Create(role);
            }

            // Criar a role 'Funcionario'
            if (!roleManager.RoleExists("Funcionario")){
                var role = new IdentityRole();
                role.Name = "Funcionario";
                roleManager.Create(role);
            }

            // Criar a role 'Dono'
            if (!roleManager.RoleExists("Dono")){
                var role = new IdentityRole();
                role.Name = "Dono";
                roleManager.Create(role);

                // criar um utilizador 'Dono'
                var user = new ApplicationUser();
                user.UserName = "blabla3@gmail.com"; //LOGIN
                user.Email = "blabla3@gmail.com";
                string userPWD = "123_Asd"; //PASSWORD
                var chkUser = userManager.Create(user, userPWD);

                //Adicionar o Utilizador à respetiva Role-Dono-
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Veterinarios");
                }


                // criar um utilizador 'Dono'
                var user2 = new ApplicationUser();
                user2.UserName = "blabla4@gmail.com"; //LOGIN
                user2.Email = "blabla4@gmail.com";
                string userPWD2 = "123_Asd"; //PASSWORD
                var chkUser2 = userManager.Create(user, userPWD);



                //Adicionar o Utilizador à respetiva Role-Dono-
                if (chkUser.Succeeded){
                    var result1 = userManager.AddToRole(user.Id, "Funcionarios");
                }

                ////////////////////////Para mais user usar Array e foreach!!!!!
            }

            // https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97
        }





    }
}
