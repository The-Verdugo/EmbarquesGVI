using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmbarquesGVI.Models.BD_A;
using EmbarquesGVI.Models.BD_B;

namespace EmbarquesGVI.Controllers
{
    public class embarquesController : Controller
    {
        private pruebasEmbarquesEntities db = new pruebasEmbarquesEntities();
        private GVIEntities dbgvi = new GVIEntities();

        // GET: embarques
        public ActionResult Index()
        {
            var embarques = db.embarques.Include(e => e.embarquesDetCajas).Include(e => e.embarquesDetTrans).Include(e => e.usuarios).Include(e => e.paqueterias).Include(e => e.tiposSol);
            return View(embarques.ToList());
        }

        // GET: embarques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            embarques embarques = db.embarques.Find(id);
            if (embarques == null)
            {
                return HttpNotFound();
            }
            return View(embarques);
        }

        // GET: embarques/Create
        public ActionResult Create()
        {
            ViewBag.DocEntry = new SelectList(db.embarquesDetCajas, "DocEntry", "DocEntry");
            ViewBag.DocEntry = new SelectList(db.embarquesDetTrans, "DocEntry", "No_trans");
            ViewBag.id_user = new SelectList(db.usuarios, "id", "nombre");
            ViewBag.paqueteria = new SelectList(db.paqueterias, "id_paq", "nombre");
            ViewBag.tipoSol = new SelectList(db.tiposSol, "id_sol", "nombre");
            ViewBag.Folio = db.embarques.OrderByDescending(x=> x.DocEntry).First().DocEntry+1;
            return View();
        }

        // POST: embaruqes/getplazas
        [HttpPost]
        public JsonResult GetPlazas(string text, string type)
        {
            if (type == "num")
            {
                var plazas = (from c in dbgvi.OSLP
                              where c.U_Plaza.Contains(text) && !(c.SlpName.Contains("(BAJA)")) && c.Active.Equals("Y")
                              select new { c.SlpName, c.U_Plaza, c.SlpCode });
                return Json(plazas, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                var plazas = (from c in dbgvi.OSLP
                              where c.SlpName.Contains(text) && !(c.SlpName.Contains("(BAJA)")) && c.Active.Equals("Y")
                              select new { c.SlpName, c.U_Plaza, c.SlpCode });
                return Json(plazas, JsonRequestBehavior.AllowGet);
            }
            
        }

        // POST: embarques/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocEntry,DocNum,plaza,almacen,fecha,tipoSol,paqueteria,NoGuia,Total_Paquetes,id_user")] embarques embarques)
        {
            if (ModelState.IsValid)
            {
                db.embarques.Add(embarques);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DocEntry = new SelectList(db.embarquesDetCajas, "DocEntry", "DocEntry", embarques.DocEntry);
            ViewBag.DocEntry = new SelectList(db.embarquesDetTrans, "DocEntry", "No_trans", embarques.DocEntry);
            ViewBag.id_user = new SelectList(db.usuarios, "id", "nombre", embarques.id_user);
            ViewBag.paqueteria = new SelectList(db.paqueterias, "id_paq", "nombre", embarques.paqueteria);
            ViewBag.tipoSol = new SelectList(db.tiposSol, "id_sol", "nombre", embarques.tipoSol);
            return View(embarques);
        }

        // GET: embarques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            embarques embarques = db.embarques.Find(id);
            if (embarques == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocEntry = new SelectList(db.embarquesDetCajas, "DocEntry", "DocEntry", embarques.DocEntry);
            ViewBag.DocEntry = new SelectList(db.embarquesDetTrans, "DocEntry", "No_trans", embarques.DocEntry);
            ViewBag.id_user = new SelectList(db.usuarios, "id", "nombre", embarques.id_user);
            ViewBag.paqueteria = new SelectList(db.paqueterias, "id_paq", "nombre", embarques.paqueteria);
            ViewBag.tipoSol = new SelectList(db.tiposSol, "id_sol", "nombre", embarques.tipoSol);
            return View(embarques);
        }

        // POST: embarques/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocEntry,DocNum,plaza,almacen,fecha,tipoSol,paqueteria,NoGuia,Total_Paquetes,id_user")] embarques embarques)
        {
            if (ModelState.IsValid)
            {
                db.Entry(embarques).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocEntry = new SelectList(db.embarquesDetCajas, "DocEntry", "DocEntry", embarques.DocEntry);
            ViewBag.DocEntry = new SelectList(db.embarquesDetTrans, "DocEntry", "No_trans", embarques.DocEntry);
            ViewBag.id_user = new SelectList(db.usuarios, "id", "nombre", embarques.id_user);
            ViewBag.paqueteria = new SelectList(db.paqueterias, "id_paq", "nombre", embarques.paqueteria);
            ViewBag.tipoSol = new SelectList(db.tiposSol, "id_sol", "nombre", embarques.tipoSol);
            return View(embarques);
        }

        // GET: embarques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            embarques embarques = db.embarques.Find(id);
            if (embarques == null)
            {
                return HttpNotFound();
            }
            return View(embarques);
        }

        // POST: embarques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            embarques embarques = db.embarques.Find(id);
            db.embarques.Remove(embarques);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
