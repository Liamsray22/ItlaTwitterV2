using DataBase.Models;

namespace Repository.Repository
{
    public class ImagenesAPIRepo : RepositoryBase<Imagenes, LIMBODBContext>
    {
        private readonly LIMBODBContext _context;     

        public ImagenesAPIRepo(LIMBODBContext context) : base(context)
        {
            _context = context;
            
        }
    }
}
