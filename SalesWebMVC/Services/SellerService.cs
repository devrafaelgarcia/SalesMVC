using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        //Ela tem que ter uma dependencia para nossa classe DBContext
        //Reandoly para depender que essa depedencia naõ seja alterada
        private readonly SalesWebMVCContext _context; 

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync(); //Operação Sicrona, rodar a app e bloquear esperando ela terminar
        }


        public async Task InsertAsync(Seller obj)
        {
            //Vai inserir esse objeto no banco de dados
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Departament).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {

  
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();

            }

            catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }

        }

        public async Task UpdateAsync(Seller obj)
        {
            //Vamos atualizar um obj do tipo Seller, vamos testar o id
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny) throw new NotFoundException("ID not found");
            //Any verifica se tem algo, se o objeto x.Id for igual o obj passado Id. 
            try {
            _context.Update(obj); //Atualiza o objto
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e) //Minha camada de serviço ela não vai progagar o erro
            {
                throw new DbConcurrencyException(e.Message); //Ela manda uma exceção da camada dela, e o SellerController, só lida com exceções da camada de serviço 
            }
            //Quando atualizamos o BD ele pode gerar um erro de concorrencia

        }

    }
}
