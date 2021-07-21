using ResumeCore.Entity.Base;

namespace ResumeCore.Interface {
    public interface IGenericRepository<T>: IRepository<T> where T : BaseEntity { }

}
