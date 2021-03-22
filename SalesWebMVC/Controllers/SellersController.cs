﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartamentService _departamentService;

        public SellersController(SellerService sellerService, DepartamentService departamentService)
        {
            _sellerService = sellerService;
            _departamentService = departamentService;

        }
        public async Task<IActionResult> Index()
        {
            //Index tem que chamar nosso method. Declarar uma dependencia
            var list = await _sellerService.FindAllAsync();

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
           
            var departaments = await _departamentService.FindAllAsync();
            var viewModel = new SellerFormViewModel() { Departaments = departaments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller) //Vai vim um obj que venho da requesição
        {
            if (!ModelState.IsValid)
            {
                var departaments = await _departamentService.FindAllAsync();
                var viewModel = new SellerFormViewModel() { Seller = seller, Departaments = departaments };
                return View(viewModel);
            }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "ID not provided" });

            var obj = await _sellerService.FindByIdAsync(id.Value); //Por ser valor opcional, em que colocar o value
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "ID not found" }); ;

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Delete(int id)
        {
            try
            {

            
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));

            }

            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "ID not provider" }); 

            var obj = await _sellerService.FindByIdAsync(id.Value); //Por ser valor opcional, em que colocar o value
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "ID not found" });

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id) //ela serve para abrir a tela para o nosso vendedor, e elar ecebe um id como argumetno
        {

            if (id == null) return RedirectToAction(nameof(Error), new { message = "ID not provider" });

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "ID not found" });

            List<Departament> departaments = await _departamentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel() {Seller = obj, Departaments = departaments };


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departaments = await _departamentService.FindAllAsync();
                var viewModel = new SellerFormViewModel() {Seller = seller, Departaments = departaments  };
                return View(viewModel);
            }

            if (id != seller.Id) return RedirectToAction(nameof(Error), new { message = "ID's mismatch" });

            try
            {

            
            await _sellerService.UpdateAsync(seller); //Ela pode lançar exceções, tanto NotFound e pode também BD
            return RedirectToAction(nameof(Index));

            }

            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }

        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel() { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            return View(viewModel);
        }
    }
}
