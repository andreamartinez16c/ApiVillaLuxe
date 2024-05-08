using ApiVillaLuxe.Data;
using ApiVillaLuxe.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVillaLuxe.Repositories
{
    public class RepositoryUsuarios
    {
        private VillaContext context;
        public RepositoryUsuarios(VillaContext context)
        {
            this.context = context;
        }

        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await
                    this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }

        public async Task<Usuario> RegisterUser(RegisterModel usu)
        {
            usu.Usuario.IdUsuario = await GetMaxIdUsuarioAsync();
            usu.Usuario.IdRol = 1;
            usu.Usuario.Salt = HelperCryptography.GenerateSalt();
            usu.Usuario.Contrasenia =
                    HelperCryptography.EncryptPassword(usu.Password, usu.Usuario.Salt);
            context.Usuarios.Add(usu.Usuario);
            await context.SaveChangesAsync();
            return usu.Usuario;
        }

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = user.Contrasenia;
                bool response =
                    HelperCryptography.CompareArrays(temp, passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            return await this.context.Usuarios.ToListAsync();
        }

        public async Task<List<Usuario>> GetUsersByRoleIdAsync(int roleId)
        {
            return await this.context.Usuarios
                .Where(u => u.IdRol == roleId)
                .ToListAsync();
        }

        public async Task <Usuario> FindUsuarioEmailPassword(string email, string password)
        {
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            string salt = user.Salt;
            byte[] contrasenia =
                    HelperCryptography.EncryptPassword(password, salt);
            return await this.context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Contrasenia == contrasenia); 
        }
    }
}
