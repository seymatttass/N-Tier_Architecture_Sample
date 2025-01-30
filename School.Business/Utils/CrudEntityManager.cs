using AutoMapper; // Entity ve DTO (Data Transfer Object) arasında dönüşüm sağlar.
using Core.Business.DTOs; // DTO'lar ile ilgili temel arayüzler ve sınıfları içerir.
using Core.Business.DTOs.Student; // Öğrenciye özel DTO sınıfları burada bulunur.
using Core.DataAccess; // Veri erişim katmanı için kullanılan temel sınıflar ve arayüzler.
using Core.Domain; // Domain (iş alanı) katmanındaki temel entity sınıfı ve ilişkileri içerir.
using School.DataAccess; // Okul projesine özel veri erişim işlemleri için kullanılan sınıflar.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Business.Utils
{
    /// <summary>
    /// Genelleştirilmiş bir CRUD yöneticisi sınıfıdır. Entity işlemlerini DTO (Data Transfer Object) ile birlikte yönetir.
    /// </summary>
    /// <typeparam name="TEntity">Temel entity sınıfı.</typeparam>
    /// <typeparam name="TEntityGetDto">Entity'nin getirme (read) işlemlerinde kullanılan DTO sınıfı.</typeparam>
    /// <typeparam name="TEntityCreateDto">Entity'nin oluşturulması (create) için kullanılan DTO sınıfı.</typeparam>
    /// <typeparam name="TEntityUpdateDto">Entity'nin güncellenmesi (update) için kullanılan DTO sınıfı.</typeparam>
    public class CrudEntityManager<TEntity, TEntityGetDto, TEntityCreateDto, TEntityUpdateDto>
        : ICrudEntityService<TEntityGetDto, TEntityCreateDto, TEntityUpdateDto> // CRUD işlemlerine ait arayüz.
        where TEntity : BaseEntity, new() // TEntity, BaseEntity sınıfından türemiş olmalıdır.
        where TEntityGetDto : IEntityGetDto, new() // Get işlemleri için kullanılan DTO türü.
        where TEntityCreateDto : IDto, new() // Create işlemleri için kullanılan DTO türü.
        where TEntityUpdateDto : IDto, new() // Update işlemleri için kullanılan DTO türü.
    {
        /// <summary>
        /// Entity ve DTO arasında dönüşüm yapmak için kullanılan AutoMapper nesnesi.
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Veri işlemlerinde kullanılacak Unit of Work (iş birimi) nesnesi.
        /// </summary>
        protected readonly IUnitOfWorks UnitOfWork;

        /// <summary>
        /// Entity'ler üzerinde CRUD işlemleri yapmak için kullanılan repository nesnesi.
        /// </summary>
        protected readonly IEntityRepository<TEntity> BaseEntityRepository;

        /// <summary>
        /// CrudEntityManager constructor'ı.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work örneği.</param>
        /// <param name="mapper">AutoMapper örneği.</param>
        public CrudEntityManager(IUnitOfWorks unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            BaseEntityRepository = UnitOfWork.GenerateRepository<TEntity>(); // TEntity tipi için repository oluştur.
        }

        /// <summary>
        /// Yeni bir entity oluşturur ve ekler.
        /// </summary>
        /// <param name="input">Oluşturulacak entity'nin DTO'su.</param>
        /// <returns>Eklenen entity'nin GetDto nesnesi.</returns>
        public virtual async Task<TEntityGetDto> AddAsync(TEntityCreateDto input)
        {
            var entity = Mapper.Map<TEntityCreateDto, TEntity>(input); // DTO'dan entity'ye dönüşüm.
            await BaseEntityRepository.AddAsync(entity); // Repository aracılığıyla entity ekleme.
            return Mapper.Map<TEntity, TEntityGetDto>(entity); // Eklenen entity'yi DTO'ya çevirerek döndür.
        }

        /// <summary>
        /// Mevcut bir entity'yi günceller.
        /// </summary>
        /// <param name="id">Güncellenecek entity'nin ID'si.</param>
        /// <param name="input">Güncelleme işlemi için kullanılan DTO.</param>
        /// <returns>Güncellenen entity'nin DTO'su.</returns>
        public virtual async Task<TEntityGetDto> UpdateAsync(Guid id, TEntityUpdateDto input)
        {
            var entity = await BaseEntityRepository.GetAsync(x => x.Id == id); // ID'ye göre entity'yi al.

            if (entity == null) // Eğer entity bulunamazsa, boş bir DTO döndür.
            {
                return new TEntityGetDto();
            }

            var updatedEntity = Mapper.Map(input, entity); // DTO'daki değişiklikleri entity'ye uygula.
            await BaseEntityRepository.UpdateAsync(updatedEntity); // Entity'yi güncelle.

            return Mapper.Map<TEntity, TEntityGetDto>(updatedEntity); // Güncellenen entity'yi DTO'ya çevirerek döndür.
        }

        /// <summary>
        /// Belirtilen ID'ye sahip bir entity'yi siler.
        /// </summary>
        /// <param name="id">Silinecek entity'nin ID'si.</param>
        public virtual async Task DeleteByIdAsync(Guid id)
        {
            await BaseEntityRepository.DeleteByIdAsync(id); // ID'ye göre entity'yi sil.
        }

        /// <summary>
        /// Belirtilen ID'ye sahip entity'yi getirir.
        /// </summary>
        /// <param name="id">Getirilecek entity'nin ID'si.</param>
        /// <returns>Entity'nin DTO'su.</returns>
        public async Task<TEntityGetDto> GetByIdAsync(Guid id)
        {
            var entity = await BaseEntityRepository.GetAsync(x => x.Id == id); // Entity'yi al.
            return await ConvertToDtoForGetAsync(entity); // DTO'ya dönüştür ve döndür.
        }

        /// <summary>
        /// Tüm entity'leri getirir.
        /// </summary>
        /// <returns>Tüm entity'lerin DTO'ları.</returns>
        public async Task<ICollection<TEntityGetDto>> GetAllAsync()
        {
            var entities = await BaseEntityRepository.GetListAsync(); // Tüm entity'leri al.
            var entityGetDtos = new List<TEntityGetDto>();

            foreach (var entity in entities.ToList()) // Tüm entity'leri DTO'ya dönüştür.
            {
                entityGetDtos.Add(await ConvertToDtoForGetAsync(entity));
            }

            return entityGetDtos; // DTO listesini döndür.
        }

        /// <summary>
        /// Entity'yi Get işlemi için DTO'ya dönüştürür.
        /// </summary>
        /// <param name="input">Dönüştürülecek entity.</param>
        /// <returns>DTO nesnesi.</returns>
        public virtual async Task<TEntityGetDto> ConvertToDtoForGetAsync(TEntity input)
        {
            return Mapper.Map<TEntity, TEntityGetDto>(input); // Entity'yi DTO'ya çevir ve döndür.
        }
    }
}
