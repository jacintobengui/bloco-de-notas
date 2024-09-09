using EditorDeTexto.Data;
using EditorDeTexto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace EditorDeTexto.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var documentos = await _context.Documentos.ToListAsync();
            return View(documentos);
        }

        public IActionResult CriarDocumento()
        {
            return View();
        }

        public async Task<IActionResult> EditarDocumento(int id)
        {
            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id);
            return View(documento);
        }

        public async Task<IActionResult> RemoverDocumento(int id)
        {
            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id);

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? pesquisa)
        {
            List<Documento> documentos = new();

            if (String.IsNullOrEmpty(pesquisa))
            {
                documentos = await _context.Documentos.ToListAsync();
                return View(documentos);
            }
            else
            {
                documentos = await _context.Documentos
                    .Where(d => d.Titulo.ToLower().Contains(pesquisa.ToLower()))
                    .ToListAsync();
            }

            return View(documentos);
        }

        [HttpPost]
        public async Task<IActionResult> CriarDocumento(Documento documentoRecebido)
        {
            if (ModelState.IsValid)
            {
                await _context.Documentos.AddAsync(documentoRecebido);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(documentoRecebido);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarDocumento(Documento documentoEditado)
        {
            if (ModelState.IsValid)
            {
                var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == documentoEditado.Id);

                documento.Titulo = documentoEditado.Titulo;
                documento.Conteudo = documentoEditado.Conteudo;
                documento.DataAlteracao = DateTime.Now;

                _context.Documentos.Update(documento);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(documentoEditado);
            }

        }
    }
}
