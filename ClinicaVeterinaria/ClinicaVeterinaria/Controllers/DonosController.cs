using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers{

    [Authorize] //forçar que só utilizadores AUTENTICADOS consigam aceder aos métodos desta classe
    //aplica-se a TODOS os métodos 


    public class DonosController : Controller{
        private VetsDB db = new VetsDB();


        // GET: Donos
        //[AllowAnonymous] //Permite o acesso de Utilizadores ANÓNIMOS aos conteúdos deste método
        //apenas deste!
        public ActionResult Index(){
            //mostra os dados apenas para os FUNCIONARIOS
            //ou para os VETERINARIOS 
            if (User.IsInRole("Veterinario") || User.IsInRole("Funcionario")){
                return View(db.Donos.ToList().OrderByDescending(d => d.Nome));
            }

            //Se chegar aqui, é porque é DONO
            return View(db.Donos.Where(d=>d.UserName.Equals(User.Identity.Name)).ToList());

        }


        // GET: Donos/Details/5
        public ActionResult Details(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Donos donos = db.Donos.Find(id);
            //criar um objeto do tipo ICollection
            //e associar esse objeto ao 'Dono'
            //donos.ListaDeAnimais= ...


            if (donos == null){
                return HttpNotFound();
            }
            return View(donos);
        }

        // GET: Donos/Create
        public ActionResult Create(){
            return View();
        }

        // POST: Donos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,NIF")] Donos dono){

            //determinar o ID a atribuir ao novo 'dono'
            int novoID = 0;

            try{
                novoID = db.Donos.Max(d => d.DonoID) + 1;
                ////outra forma
                //novoID = db.Donos.Last().DonoID + 1;
                ////outra forma
                //novoID = (from d in db.Donos
                //          orderby d.DonoID descending
                //          select d.DonoID).FirstOrDefault()+1;
                ////outra forma
                //novoID = db.Donos.OrderByDescending(d => d.DonoID).FirstOrDefault().DonoID + 1;
            }
            catch (Exception){
                //não existe dados na BD
                //o MAX devolve Null
                novoID=1;
            }
            



            //atribuir o novo ID ao 'dono'
            dono.DonoID = novoID;

            try{
                if (ModelState.IsValid)
                {//confronta se os dados a ser introduzidos estão consistentes com o Model

                    db.Donos.Add(dono);//adicionar um novo 'dono' 
                    db.SaveChanges();//guardar as alterações

                    return RedirectToAction("Index");//redericionar para a página de início
                }
            }catch (Exception ex){
                ModelState.AddModelError("", string.Format("Ocorreu um erro na operação de guardar um novo dono..."));
                /*adicionar a uma classe ERRO
                 * -id
                 * -timestamp
                 * -operação que gerou o erro
                 * -mensagem de erro(ex.Message)
                 * -qual o User que gerou o erro
                 * -...
                 * 
                 * enviar email ao utilizador 'Admin' a avisar da ocorrência do erro
                 * 
                 * outras coisas consideradas importantes...
                 */
            }
            

            //se houver problemas, volta para a View do Create com os danos do 'dono'
            return View(dono);
        }

        // GET: Donos/Edit/5
        public ActionResult Edit(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donos donos = db.Donos.Find(id);
            if (donos == null){
                return HttpNotFound();
            }
            return View(donos);
        }

        // POST: Donos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DonoID,Nome,NIF")] Donos donos){
            if (ModelState.IsValid){
                db.Entry(donos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donos);
        }

        // GET: Donos/Delete/5
        public ActionResult Delete(int? id){
            //se não foi fornecido o ID do 'Dono'...
            if (id == null){
                //redireciono o utilizador para a lista de Donos
                return RedirectToAction("Index");
            }

            //vai à procura do 'Dono', cujo ID foi fornecido
            Donos dono = db.Donos.Find(id);

            //se o 'dono' associado ao ID fornecido não existe...
            if (dono == null){
                //redireciono o utilizador para a lista de Donos
                return RedirectToAction("Index");
            }
            //mostra os dados do 'Dono'
            return View(dono);
        }

        // POST: Donos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id){
            //procura o 'dono' na base dados
            Donos dono = db.Donos.Find(id);
            try {
                //marcar o 'dono' para eliminação
                db.Donos.Remove(dono);

                //efetuar um 'commit' ao comando anterior
                db.SaveChanges();

                return RedirectToAction("Index");

            }catch (Exception){
                ModelState.AddModelError("",
                   string.Format("Occorreu um ERRO na eliminação do Dono com ID={0}-{1}", id, dono.Nome)
                );

                //invoca a View, com os dados do dono
                return View(dono);
            }



        }

        protected override void Dispose(bool disposing){
            if (disposing){
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
